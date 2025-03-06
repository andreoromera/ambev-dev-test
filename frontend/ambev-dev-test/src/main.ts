import { enableProdMode, importProvidersFrom } from '@angular/core';

import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideZxvbnServiceForPSM } from 'angular-password-strength-meter/zxcvbn';
import { provideNgxMask } from "ngx-mask";
import { provideToastr } from "ngx-toastr";
import { AppRoutingModule } from './app/app-routing.module';
import { AppComponent } from './app/app.component';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(BrowserModule, AppRoutingModule),
    provideAnimations(),
    provideZxvbnServiceForPSM(),
    provideNgxMask({}),
    provideToastr()
  ]
}).catch((err) => console.error(err));
