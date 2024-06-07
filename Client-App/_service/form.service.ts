import { Injectable } from "@angular/core";
import { FormControl, FormGroup, Validator, ValidatorFn, Validators } from "@angular/forms"

@Injectable({
  providedIn: 'root'
})
export class FormService {

  constructor() { }

  public control(): FormControl {
    return new FormControl('');
  }

  public required(): FormControl {
    return new FormControl('', [Validators.required]);
  }

  public email(): FormControl {
    return new FormControl('', [Validators.required, Validators.email]);
  }

  public password(): FormControl {
    return new FormControl('', [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])/),]);
  }

  public passPattern(): ValidatorFn {
    return Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])/);
  }

  public require(): ValidatorFn {
    return Validators.required;
  }

  public match(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {

      const c = formGroup.controls[controlName];
      const mc = formGroup.controls[matchingControlName];

      if (mc.errors && !matchingControlName.match)
        return;

      c.value !== mc.value ? mc.setErrors({ nomatch: true }) : mc.setErrors(null);
    }
  }

}

