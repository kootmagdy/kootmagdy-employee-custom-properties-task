import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf, NgFor } from '@angular/common';
import { PropertyService } from '../../core/services/property.service';
import { CreatePropertyDto, PropertyType } from '../../core/models/property.model';

@Component({
  selector: 'app-property-form',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, NgFor],
  templateUrl: './property-form.component.html',
  styleUrls: ['./property-form.component.scss']
})
export class PropertyFormComponent implements OnInit {
  form!: FormGroup;
  types: PropertyType[] = ['String', 'Integer', 'Date', 'Dropdown'];
  submitting = false;
  successMsg = '';
  errorMsg = '';

  constructor(private fb: FormBuilder, private propSvc: PropertyService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      type: ['String', Validators.required],
      isRequired: [false],
      dropdownOptions: this.fb.array<FormControl<string>>([])
    });

    this.form.get('type')!.valueChanges.subscribe(t => {
      if (t === 'Dropdown') {
        if (this.dropdownOptions.length === 0) this.addOption();
      } else {
        this.dropdownOptions.clear();
      }
    });
  }

  get dropdownOptions(): FormArray<FormControl<string>> {
    return this.form.get('dropdownOptions') as FormArray<FormControl<string>>;
  }

  addOption(): void {
    this.dropdownOptions.push(this.fb.control('', { nonNullable: true, validators: [Validators.required] }));
  }

  removeOption(i: number): void {
    this.dropdownOptions.removeAt(i);
  }

  submit(): void {
    this.successMsg = '';
    this.errorMsg = '';

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dto: CreatePropertyDto = {
      name: this.form.value.name.trim(),
      type: this.form.value.type,
      isRequired: this.form.value.isRequired,
      dropdownOptions: this.form.value.type === 'Dropdown'
        ? (this.form.value.dropdownOptions as string[]).map(v => v.trim()).filter(Boolean)
        : undefined
    };

    this.submitting = true;
    this.propSvc.create(dto).subscribe({
      next: (res) => {
        this.successMsg = `Property "${res.name}" created (id: ${res.id}).`;
        this.submitting = false;
        this.form.reset({ name: '', type: 'String', isRequired: false });
        this.dropdownOptions.clear();
      },
      error: (err) => {
        this.submitting = false;
        this.errorMsg = (err?.error as string) || 'Failed to create property.';
      }
    });
  }
}
