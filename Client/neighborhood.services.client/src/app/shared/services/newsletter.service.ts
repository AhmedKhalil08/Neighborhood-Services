import { Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root',
})
export class NewsletterService {
private Endpoint = '/api/Newsletter';
constructor(private apiService: ApiService) { }
newssubscribers?: string[];

//to get all subscribers
  getAll(): Observable<string[]> {
      return this.apiService.get<string[]>(this.Endpoint);
    }

  //to add a new subscriber
   subscribe(email:string): Observable<string[]> {
      return this.apiService.post(this.Endpoint,email);
    }

  //to publish a new letter
   publish(subject:string,content:string): Observable<string[]> {
      return this.apiService.post(this.Endpoint+'/SendNewsLetter',{subject,content});
    }




}



