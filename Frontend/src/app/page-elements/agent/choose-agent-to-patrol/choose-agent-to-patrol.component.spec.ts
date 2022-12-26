import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseAgentToPatrolComponent } from './choose-agent-to-patrol.component';

describe('ChooseAgentToPatrolComponent', () => {
  let component: ChooseAgentToPatrolComponent;
  let fixture: ComponentFixture<ChooseAgentToPatrolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseAgentToPatrolComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseAgentToPatrolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
