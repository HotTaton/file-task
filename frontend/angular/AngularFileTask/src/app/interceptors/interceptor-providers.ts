import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { APIInterceptor } from './api-interceptor/apiinterceptor';

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: APIInterceptor, multi: true },
];