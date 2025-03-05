/* eslint-disable @angular-eslint/directive-selector */
import { Directive, Input } from "@angular/core";
import { UntypedFormGroup, NG_VALIDATORS, ValidationErrors, Validator } from "@angular/forms";

@Directive({
  selector: "[mustMatch]",
  providers: [{ provide: NG_VALIDATORS, useExisting: MustMatchDirective, multi: true }]
})
export class MustMatchDirective implements Validator {
  @Input("mustMatch") mustMatchFields: string[] = [];

  validate (formGroup: UntypedFormGroup): ValidationErrors {
    return this.mustMatch(this.mustMatchFields[0], this.mustMatchFields[1])(formGroup);
  }

  mustMatch (controlName: string, matchingControlName: string): any {
    return (formGroup: UntypedFormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      // return null if controls haven't initialised yet
      if (!control || !matchingControl) {
        return null;
      }

      // return null if another validator has already found an error on the matchingControl
      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
        return null;
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }

      return;
    };
  }
}
