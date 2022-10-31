import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentsForSaleComponent } from './agents-for-sale.component';

describe('AgentsForSaleComponent', () => {
  let component: AgentsForSaleComponent;
  let fixture: ComponentFixture<AgentsForSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AgentsForSaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentsForSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
