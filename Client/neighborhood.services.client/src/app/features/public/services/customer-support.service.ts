import { Injectable,signal,computed,NgZone,inject, Signal } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import * as signalR from '@microsoft/signalr';
import {TicketDto} from '../../public/models/ticket-dto';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class CustomerSupportService {
  private hubConnection: signalR.HubConnection;

  public receivedTickets!:Signal<TicketDto[]>
  incomingTickets$ = new BehaviorSubject<TicketDto[]>([]);

/**
 *
 */
userEmail:string="";
constructor() {
this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7228/customerServiceHub')
      .build();

 this.hubConnection.start().catch(err => console.error('SignalR error: ', err));
       this.hubConnection.on('ReceiveTicket', (ticket: any) => {
    const updated = [ticket, ...this.incomingTickets$.value];
     console.log("Updated list: ", updated);
      this.incomingTickets$.next(updated);

      console.log("from receive ticket");
      console.log(ticket);
    });

     this.hubConnection.on('TicketChat', (message: any) => {
      console.log("from receive chat");
      console.log(message);
    });
  
}

sendTicket(ticket:TicketDto):void{
  this.hubConnection.invoke("SendLiveTicket",ticket.senderEmail,ticket)
}

sendMessage(mssg:string):void{
  this.hubConnection.invoke("SendGroupMessage",mssg)
}

//when user enters his dispute, a notification is sent to admins, a new conversation of disputes chat component is created

}
