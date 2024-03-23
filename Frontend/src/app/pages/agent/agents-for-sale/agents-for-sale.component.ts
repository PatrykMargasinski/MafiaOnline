import { Component, OnInit } from '@angular/core';
import { AgentForSale } from 'src/app/models/agent/agent.models';
import { AgentActionsService } from 'src/app/services/agent/agent-actions.service';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';


@Component({
  selector: 'app-agents-for-sale',
  templateUrl: './agents-for-sale.component.html',
  styleUrls: ['./agents-for-sale.component.css']
})

export class AgentsForSaleComponent implements OnInit {

  constructor(private shared: AgentService, private agentActions: AgentActionsService, private tokenService: TokenService) { }
  AgentsForSale:AgentForSale[];

  ngOnInit(): void {
    this.refresh();
  }

  refresh(){
    this.shared.getAgentsForSaleList().subscribe(data=>{
      this.AgentsForSale=data;
    });
  }

  recruitAgent(agentId: number): void
  {
    if(confirm('Are you sure??')){
        this.agentActions.recruitAgent(agentId).subscribe(data=>{
          alert(data.toString());
          this.refresh();
        })
    }
  }
}
