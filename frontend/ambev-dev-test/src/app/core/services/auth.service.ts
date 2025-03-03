import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticated = false;
  private authSecretKey = 'auth';
  private baseAddress = 'https://localhost:7199';

  constructor() {
    this.isAuthenticated = !!localStorage.getItem(this.authSecretKey);
  }

  async login(email: string, password: string) {
    try {
      const response = await fetch(`${this.baseAddress}/auth/signin`, {
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

      localStorage.setItem(this.authSecretKey, JSON.stringify(await response.json()));
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
    return JSON.parse(localStorage.getItem(this.authSecretKey)!);
  }

  logout(): void {
    localStorage.removeItem(this.authSecretKey);
    this.isAuthenticated = false;
  }
}
