export type PropertyType = 'String' | 'Integer' | 'Date' | 'Dropdown';

export interface PropertyResponse {
  id: number;
  name: string;
  type: PropertyType;
  isRequired: boolean;
  dropdownOptions?: string[] | null;
}

export interface CreatePropertyDto {
  name: string;
  type: PropertyType;
  isRequired: boolean;
  dropdownOptions?: string[]; 
}
