import { Component, OnInit } from '@angular/core';
import { Boss } from 'src/app/models/boss/boss.models';
import { BossService } from 'src/app/services/boss/boss.service';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-boss-datas',
  templateUrl: './about-boss.component.html',
  styleUrls: ['./about-boss.component.css']
})
export class AboutBossComponent implements OnInit {

  boss: Boss ={} as Boss

  constructor(private bossService: BossService, private tokenService: TokenService) { }

  ngOnInit(): void {
    this.bossService.getBoss(Number(this.tokenService.getBossId())).subscribe(data=>{
      this.boss=data;
    })
  }
}
