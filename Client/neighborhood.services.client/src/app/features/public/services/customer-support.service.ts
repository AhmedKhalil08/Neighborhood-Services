import { Injectable,signal,computed,NgZone,inject } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import * as signalR from '@microsoft/signalr';
import { MessageDto } from '../../../core/models/message-dto';
import { MessageSelectedDto } from '../../../core/models/message-selected-dto';
import { MessagesService } from '../../customer/services/messages.service';

@Injectable({
  providedIn: 'root',
})
export class CustomerSupportService {
  private hubConnection: signalR.HubConnection;

/**
 *
 */
userEmail:string="";
constructor() {
this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7228/CustomerSupportHub')
      .build();

       this.hubConnection.on('ReceiveCustomerSupport', (message: any) => {
      console.log("from receive");

    this.hubConnection.invoke('JoinGroup', this.userEmail)
 
    
    });
  
}

//when user enters his dispute, a notification is sent to admins, a new conversation of disputes chat component is created

}
