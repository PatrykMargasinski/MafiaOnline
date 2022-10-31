import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseAgentOnMissionComponent } from './choose-agent-on-mission.component';

describe('ChooseAgentOnMissionComponent', () => {
  let component: ChooseAgentOnMissionComponent;
  let fixture: ComponentFixture<ChooseAgentOnMissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseAgentOnMissionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseAgentOnMissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
