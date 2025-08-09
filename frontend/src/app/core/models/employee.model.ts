import { PropertyType } from './property.model';

export interface EmployeePropWithValue {
  propertyId: number;
  propertyName: string;
  type: PropertyType;
  isRequired: boolean;
  dropdownOptions?: string[] | null;
  value?: string | null;
}

export interface EmployeeResponse {
  id: number;
  code: string;
  name: string;
  properties: EmployeePropWithValue[];
}

export interface EmployeePropertyValueDto {
  propertyId: number;
  value?: string | null;
}

export interface CreateEmployeeDto {
  code: string;
  name: string;
  propertyValues?: EmployeePropertyValueDto[];
}
