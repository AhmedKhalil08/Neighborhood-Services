import { Component } from '@angular/core';
import {NewsletterService} from'../../services/newsletter.service'

@Component({
  selector: 'app-newsletter-subscribe',
  imports: [],
  templateUrl: './newsletter-subscribe.component.html',
  styleUrl: './newsletter-subscribe.component.css',
})
export class NewsletterSubscribeComponent {
  /**
   *
   */
  constructor(private myService:NewsletterService) {}
email:string='';
  subs(inputMail:string):void{
    this.myService.subscribe(inputMail);
    console.log("subscribed:"+inputMail);
  }
    
    
    
  
}
