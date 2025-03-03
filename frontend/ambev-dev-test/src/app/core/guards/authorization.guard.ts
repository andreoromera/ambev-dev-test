import { inject } from "@angular/core";
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from "../services/auth.service";

export const authorizationGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticatedUser()) {
    return true;
  } else {
    // Redirect to the login page if the user is not authenticated
    router.navigate(['/login']);
    return false;
  }
};
