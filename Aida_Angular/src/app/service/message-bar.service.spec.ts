import { TestBed } from '@angular/core/testing';

import { MessageBarService } from './message-bar.service';

describe('MessageBarService', () => {
  let service: MessageBarService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MessageBarService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
