import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HowToPlayAgentsComponent } from './how-to-play-agents.component';

describe('HowToPlayAgentsComponent', () => {
  let component: HowToPlayAgentsComponent;
  let fixture: ComponentFixture<HowToPlayAgentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HowToPlayAgentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HowToPlayAgentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
