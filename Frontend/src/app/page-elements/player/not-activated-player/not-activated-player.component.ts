import { Component, OnInit } from '@angular/core';
import { PlayerService } from 'src/app/services/player/player.service';

@Component({
  selector: 'app-not-activated-player',
  templateUrl: './not-activated-player.component.html',
  styleUrls: ['./not-activated-player.component.css']
})
export class NotActivatedPlayerComponent implements OnInit {

  constructor(private playerService: PlayerService) { }

  ngOnInit(): void {
  }


  resendLink()
  {
    this.playerService.resendActivationLink().subscribe(x=>
      {
        alert("Link resended");
      }
    )
  }

}
