import { Component,Input,OnInit } from '@angular/core';
import { TicketDto } from '../../../models/ticket-dto';
import {CustomerSupportService} from '../../../services/customer-support.service'

@Component({
  selector: 'app-customer-support',
  imports: [],
  templateUrl: './customer-support.component.html',
  styleUrl: './customer-support.component.css',
})
export class CustomerSupportComponent {
    @Input({ required: true })
  ticket!: TicketDto;
 
  constructor(private myService:CustomerSupportService) {
   
  }
  ngOnInit(){
    setTimeout(() => {
  this.myService.sendTicket(this.ticket)
}, 5000);
  }
}
