import { Component, OnInit } from '@angular/core';
import { Agent, AgentQuery } from 'src/app/models/agent/agent.models';
import { TableHeader } from 'src/app/models/helpers/TableHeader';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-agent-list',
  templateUrl: './agent-list.component.html',
  styleUrls: ['./agent-list.component.css']
})
export class AgentListComponent implements OnInit {

  agents: Agent[] = [];
  filteredAgents: Agent[] = [];
  filters: AgentQuery = new AgentQuery();

  tableHeaders: TableHeader[] = [
    { Value: "FullName", DisplayValue: "Name", SortValue: "LastName", Sortable: true },
    { Value: "Strength", DisplayValue: "Strength", Sortable: true },
    { Value: "Dexterity", DisplayValue: "Dexterity", Sortable: true },
    { Value: "Intelligence", DisplayValue: "Intelligence", Sortable: true },
    { Value: "StateName", DisplayValue: "State", Sortable: true },
  ];

  constructor(private agentService: AgentService) {}

  ngOnInit() {
    this.getAgents();
  }

  getAgents() {
    console.log(this.filters);
    this.agentService.getAgentsByQuery(this.filters).subscribe((data: Agent[]) => {
      this.agents = data;
      this.filteredAgents = this.agents;
    });
  }

  clearFilters() {
    this.filters = new AgentQuery();
    this.getAgents();
  }

  applyFilters() {
    this.getAgents();
  }
}
