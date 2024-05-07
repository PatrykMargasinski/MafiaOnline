import { Component, OnDestroy, OnInit } from '@angular/core';
import { Agent, AgentQuery } from 'src/app/models/agent/agent.models';
import { TableHeader } from 'src/app/models/helpers/TableHeader';
import { State } from 'src/app/models/state/state';
import { AgentService } from 'src/app/services/agent/agent.service';
import { StateService } from 'src/app/services/state/state.service';

@Component({
  selector: 'app-agent-list',
  templateUrl: './agent-list.component.html',
  styleUrls: ['./agent-list.component.css']
})
export class AgentListComponent implements OnInit, OnDestroy {

  agents: Agent[] = [];
  filteredAgents: Agent[] = [];
  filters: AgentQuery = new AgentQuery();
  pageNumbers: number[] = []
  pageSize: number = 5;
  agentStates: State[] = []
  timer: NodeJS.Timeout

  tableHeaders: TableHeader[] = [
    { Value: "FullName", DisplayValue: "Name", SortValue: "LastName", Sortable: true },
    { Value: "Strength", DisplayValue: "Strength", Sortable: true },
    { Value: "Dexterity", DisplayValue: "Dexterity", Sortable: true },
    { Value: "Intelligence", DisplayValue: "Intelligence", Sortable: true },
    { Value: "StateName", Value2: "SubstateName", DisplayValue: "State", Sortable: false },
    { Value: "FinishTime", DisplayValue: "Finish time", Sortable: false, IsTimeRemaining: true },
  ];

  constructor(private agentService: AgentService, private stateService: StateService) {}


  ngOnInit() {
    this.getAgents();
  }

  ngOnDestroy(): void {
    clearInterval(this.timer);
  }

  handleActionResponse(response: string)
  {
    if(response)
      alert(response);
    this.applyFilters();
  }

  getAgents() {
    this.agentService.getAgentsByQuery(this.filters).subscribe((data: Agent[]) => {
      this.agents = data;
      this.filteredAgents = this.agents;
      for(let i=0; i<Math.ceil(this.filteredAgents.length/this.pageSize); i++)
        this.pageNumbers.push(i*this.pageSize);
      clearInterval(this.timer);
      this.timer = setInterval(this.updateTime, 1000);
    });
    this.stateService.getAvailableAgentStates().subscribe(x=>
      {
        this.agentStates = x;
      });
  }

  clearFilters() {
    this.filters = new AgentQuery();
    this.getAgents();
  }

  applyFilters() {
    this.getAgents();
  }

  getSecondsRemaining(target: string) : number
  {
    const currentTime = new Date().getTime();
    const targetTime = new Date(target).getTime();
    if (targetTime > currentTime) {
      const timeDifference = targetTime - currentTime;
      const secondsRemaining = Math.floor(timeDifference / 1000);
      return secondsRemaining;
    } else {
      return 0;
    }
  }

  updateTime()
  {
    //invokes getSecondsRemaining
    var timeHeaders = this.tableHeaders.filter(x=>x.IsTimeRemaining == true);
  }
}
