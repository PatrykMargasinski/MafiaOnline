import { Component, Input, OnInit } from '@angular/core';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-map-element',
  templateUrl: './map-element.component.html',
  styleUrls: ['./map-element.component.css']
})
export class MapElementComponent implements OnInit {

  constructor(private tokenService: TokenService) { }

  @Input() bossId: number
  @Input() ifPlayerImage: string
  @Input() ifNotPlayerImage: string

  ngOnInit(): void {
  }

  isYourMapElement()
  {
    return this.tokenService.getBossId() == this.bossId;
  }

}
