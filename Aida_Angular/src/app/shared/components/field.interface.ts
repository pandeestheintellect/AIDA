export interface Validator {
  name: string;
  validator: any;
  message: string;
}
export interface FieldConfig {
  label?: string;
  name?: string;
  inputType?: string;
  options?: string[];
  collections?: any;
  type: string;
  value?: any;
  controlNumber?: any;
  fxSizePer?:any;
  format?:any;
  validations?: Validator[];
}
