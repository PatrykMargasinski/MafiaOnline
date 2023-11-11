import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentActionsComponent } from './agent-actions.component';

describe('AgentActionsComponent', () => {
  let component: AgentActionsComponent;
  let fixture: ComponentFixture<AgentActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AgentActionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
