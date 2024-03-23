import { TestBed } from '@angular/core/testing';

import { AgentActionsService } from './agent-actions.service';

describe('AgentActionsService', () => {
  let service: AgentActionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentActionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
