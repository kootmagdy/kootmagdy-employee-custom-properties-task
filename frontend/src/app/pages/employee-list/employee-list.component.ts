import { Component, OnInit, computed, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../../core/services/employee.service';
import { EmployeeResponse } from '../../core/models/employee.model';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-employee-list',
  imports: [CommonModule,RouterLink],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
  loading = signal(true);
  error   = signal<string | null>(null);
  raw     = signal<EmployeeResponse[]>([]);
  q       = signal('');                // search text
  dept    = signal<string>('');        // department filter (optional)
  
  // استنتاج قيم الأقسام من الخصائص لو موجودة
  departments = computed<string[]>(() => {
    const set = new Set<string>();
    for (const e of this.raw()) {
      const dep = e.properties.find(p => p.propertyName.toLowerCase() === 'department')?.value;
      if (dep) set.add(dep);
    }
    return Array.from(set).sort();
  });

  // فلترة في الواجهة (اسم/كود + القسم)
  list = computed(() => {
    const term = this.q().trim().toLowerCase();
    const dep  = this.dept();
    return this.raw().filter(e => {
      const hitsText =
        !term ||
        e.name.toLowerCase().includes(term) ||
        e.code.toLowerCase().includes(term);
      const hitsDept =
        !dep ||
        e.properties.find(p => p.propertyName.toLowerCase() === 'department')?.value === dep;
      return hitsText && hitsDept;
    });
  });

  constructor(private empSvc: EmployeeService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.empSvc.getAll().subscribe({
      next: (res) => { this.raw.set(res); this.loading.set(false); },
      error: (e) => { this.error.set('Failed to load employees'); this.loading.set(false); }
    });
  }

  clearFilters(): void {
    this.q.set('');
    this.dept.set('');
  }
}
