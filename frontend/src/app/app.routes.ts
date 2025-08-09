// app.routes.ts
import { Routes } from '@angular/router';
import { PropertyFormComponent } from './pages/property-form/property-form.component';
import { EmployeeFormComponent } from './pages/employee-form/employee-form.component';
import { EmployeeListComponent } from './pages/employee-list/employee-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'properties/new', pathMatch: 'full' },
  { path: 'properties/new', component: PropertyFormComponent },
  { path: 'employees/new', component: EmployeeFormComponent },
  { path: 'employees', component: EmployeeListComponent}

];
