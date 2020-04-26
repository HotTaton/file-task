import { Injectable, Inject } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'
import { ServiceResponse } from 'src/app/contracts/response';

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            map(event => {
                if (event instanceof HttpResponse) {
                    if (event.body) {
                        var castedBody = event.body as ServiceResponse<any>;
                        event = event.clone({ body: castedBody.item });
                    }
                }                
                return event;
            })
        );        
    }
}
