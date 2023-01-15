import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovingAgentsComponent } from './moving-agents.component';

describe('MovingAgentsComponent', () => {
  let component: MovingAgentsComponent;
  let fixture: ComponentFixture<MovingAgentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MovingAgentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MovingAgentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
