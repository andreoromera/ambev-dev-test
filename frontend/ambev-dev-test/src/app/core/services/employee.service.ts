import { inject, Injectable } from '@angular/core';
import { AuthService } from "./auth.service";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private authService = inject(AuthService);
  private baseAddress = 'https://localhost:7199';

  async filter(firstName?: string, lastName?: string) {
    try {
      const user = this.authService.getUserData()!;

      const headers = new Headers();
      headers.append('Authorization', `Bearer ${user.token}`);

      const params = new URLSearchParams();
      if (firstName) params.append('firstname', firstName);
      if (lastName) params.append('lastname', lastName);

      const response = await fetch(`${this.baseAddress}/employee/search?${params}`, { headers: headers });

      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }

      return await response.json();
    } catch (err) {
      console.error(err.message);
    }
  }
}
