import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { EmployeeService } from '../../core/services/employee.service';
import { PropertyService } from '../../core/services/property.service';
import { PropertyResponse } from '../../core/models/property.model';
import { CreateEmployeeDto } from '../../core/models/employee.model';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-employee-form',
  imports: [CommonModule, ReactiveFormsModule,RouterLink],
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss']
})
export class EmployeeFormComponent implements OnInit {
  form!: FormGroup;
  props: PropertyResponse[] = [];
  loading = true;
  submitting = false;
  ok = ''; err = '';

  constructor(private fb: FormBuilder, private empSvc: EmployeeService, private propSvc: PropertyService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      props: this.fb.group({}) // dynamic bag
    });

    this.propSvc.getAll().subscribe({
      next: (list) => {
        this.props = list;
        const bag = this.form.get('props') as FormGroup;

        for (const p of this.props) {
          const validators = p.isRequired ? [Validators.required] : [];
          // كل الأنواع هتبقى نص عند الإرسال (الباك إند بيعمل الفاليديشن)
          bag.addControl(p.id.toString(), new FormControl<string | null>(null, validators));
        }

        this.loading = false;
      },
      error: () => { this.err = 'Failed to load properties'; this.loading = false; }
    });
  }

  submit(): void {
    this.ok = ''; this.err = '';
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    const bag = this.form.value.props as Record<string, string | null>;
    const values = Object.entries(bag)
      .filter(([_, v]) => v !== null && v !== '')
      .map(([k, v]) => ({ propertyId: +k, value: v as string }));

    const dto: CreateEmployeeDto = {
      code: this.form.value.code.trim(),
      name: this.form.value.name.trim(),
      propertyValues: values
    };

    this.submitting = true;
    this.empSvc.create(dto).subscribe({
      next: (res) => {
        this.ok = `Employee "${res.name}" created (id: ${res.id}).`;
        this.submitting = false;
        this.form.reset();
        (this.form.get('props') as FormGroup).reset();
      },
      error: (e) => {
        this.submitting = false;
        this.err = e?.error || 'Failed to create employee';
      }
    });
  }
}
