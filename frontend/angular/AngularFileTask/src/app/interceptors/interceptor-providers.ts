import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { APIInterceptor } from './api-interceptor/apiinterceptor';
import { ResponseInterceptor } from './response-interceptor/response-interceptor';

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: APIInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: ResponseInterceptor, multi: true },
];