import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreatePropertyDto, PropertyResponse } from '../models/property.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PropertyService {
  private readonly base = '/api/properties';

  constructor(private http: HttpClient) {}

  getAll(): Observable<PropertyResponse[]> {
    return this.http.get<PropertyResponse[]>(this.base);
  }

  create(dto: CreatePropertyDto): Observable<PropertyResponse> {
    return this.http.post<PropertyResponse>(this.base, dto);
  }
}
