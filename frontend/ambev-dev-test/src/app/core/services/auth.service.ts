import { Injectable } from '@angular/core';
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticated = false;
  private authSecretKey = 'auth';

  constructor() {
    this.isAuthenticated = !!sessionStorage.getItem(this.authSecretKey);
  }

  async login(email: string, password: string) {
    try {
      const response = await fetch(`${environment.apiUrl}/auth/signin`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email, password })
      });

      if (!response.ok) {
        console.log(response);
        throw new Error(`Response status: ${response.status}`);
      }

      sessionStorage.setItem(this.authSecretKey, JSON.stringify(await response.json()));
      this.isAuthenticated = true;
      return true;
    } catch (error) {
      console.error(error.message);
      return false;
    }
  }

  isAuthenticatedUser(): boolean {
    return this.isAuthenticated;
  }

  getUserData() {
    return JSON.parse(sessionStorage.getItem(this.authSecretKey)!);
  }

  logout(): void {
    sessionStorage.removeItem(this.authSecretKey);
    this.isAuthenticated = false;
  }
}
