import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentInviteComponent } from './document-invite.component';

describe('DocumentInviteComponent', () => {
  let component: DocumentInviteComponent;
  let fixture: ComponentFixture<DocumentInviteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentInviteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentInviteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
