import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  CreateEmployeeDto,
  EmployeeResponse
} from '../models/employee.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private readonly base = '/api/employees';

  constructor(private http: HttpClient) {}

  getAll(): Observable<EmployeeResponse[]> {
    return this.http.get<EmployeeResponse[]>(this.base);
  }

  getById(id: number): Observable<EmployeeResponse> {
    return this.http.get<EmployeeResponse>(`${this.base}/${id}`);
  }

  create(dto: CreateEmployeeDto): Observable<EmployeeResponse> {
    return this.http.post<EmployeeResponse>(this.base, dto);
  }
}
