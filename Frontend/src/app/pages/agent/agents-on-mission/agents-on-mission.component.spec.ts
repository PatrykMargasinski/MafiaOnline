import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentsOnMissionComponent } from './agents-on-mission.component';

describe('AgentsOnMissionComponent', () => {
  let component: AgentsOnMissionComponent;
  let fixture: ComponentFixture<AgentsOnMissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AgentsOnMissionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentsOnMissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
