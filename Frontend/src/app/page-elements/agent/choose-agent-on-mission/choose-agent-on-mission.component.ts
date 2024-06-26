import { MapUtils } from './../../../utils/map-utils';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Agent } from 'src/app/models/agent/agent.models';
import { MapOperations } from 'src/app/models/map/mapField.models';
import { Mission } from 'src/app/models/mission/mission.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { MissionService } from 'src/app/services/mission/mission.service';

@Component({
  selector: 'app-choose-agent-on-mission',
  templateUrl: './choose-agent-on-mission.component.html',
  styleUrls: ['./choose-agent-on-mission.component.css']
})
export class ChooseAgentOnMissionComponent implements OnInit {

  @Input() missionId: number
  @Input() mapElementId: number
  mission: Mission
  chosenAgent: Agent = null
  @Output() someEvent = new EventEmitter<number>();
  agents: Agent[]
  agentPathSet: boolean = false

  constructor(
    private agentService: AgentService,
    private missionService: MissionService,
    private tokenService: TokenService,
    private mapUtils: MapUtils
    ) {

    }


  handleCheckboxChange(agent: Agent)
  {
    this.chosenAgent = agent
  }

  ngOnInit(): void {
    this.getAgents();
    if(this.missionId!=null)
      this.getMissionById();
    if(this.mapElementId!=null)
      this.getMissionByMapElement();
    this.agentPathSet = this.mapUtils.pathExists();
  }

  getAgents(){
    this.agentService.getAvailableAgents().subscribe(data=>{
      this.agents = data
    })
  }

  getMissionById(){
    this.missionService.getMissionById(this.missionId).subscribe(data=>{
      this.mission = data
    })
  }

  getMissionByMapElement(){
    this.missionService.getMissionByMapElement(this.mapElementId).subscribe(data=>{
      this.mission = data
      this.missionId = data.Id
    })
  }

  moveOnMission(agentId: number){
    let path = this.mapUtils.getPath()
    this.missionService.moveOnMission(agentId, this.missionId, path).subscribe(it=>{
      alert("Agent sent on mission")
      this.someEvent.next(MapOperations.MissionChosen)
    })
  }

  setAgentPath(){
    this.mapUtils.clearPath();
      this.someEvent.next(MapOperations.StartSettingMissionPath)
  }
}


