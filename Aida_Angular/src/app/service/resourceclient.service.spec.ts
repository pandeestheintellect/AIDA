import { TestBed } from '@angular/core/testing';

import { ResourceclientService } from './resourceclient.service';

describe('ResourceclientService', () => {
  let service: ResourceclientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResourceclientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
