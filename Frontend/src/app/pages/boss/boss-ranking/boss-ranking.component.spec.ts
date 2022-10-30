import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BossRankingComponent } from './boss-ranking.component';

describe('BossRankingComponent', () => {
  let component: BossRankingComponent;
  let fixture: ComponentFixture<BossRankingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BossRankingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BossRankingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
