import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Boss } from 'src/app/models/boss/boss.models';
import { BossService } from 'src/app/services/boss/boss.service';

@Component({
  selector: 'app-boss-ranking',
  templateUrl: './boss-ranking.component.html',
  styleUrls: ['./boss-ranking.component.css']
})
export class BossRankingComponent implements OnInit {

  constructor(private shared: BossService, private router: Router) { }

  BossList: Boss[];

  ngOnInit(): void {
    this.refreshBossList();
  }

  sendMessage(bossName: string){
    this.router.navigate(["/message/send"], { queryParams: { bossName: bossName }});
  }

  refreshBossList(){
    this.shared.getBestBosses(0,10).subscribe(x=>{
      this.BossList=x;
    })
  }

}
