import { Component, inject } from '@angular/core';
import { NgForm } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { AuthService } from "../core/services/auth.service";
import { SharedModule } from "../theme/shared/shared.module";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  email = '';
  password = '';

  async onSubmit(myform: NgForm) {
    if (myform.valid) {
      const success = await this.authService.login(this.email, this.password);

      if (success) {
        this.router.navigate(["/"]);
      }
    }
  }
}
