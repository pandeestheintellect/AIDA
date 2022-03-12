import { NgModule } from '@angular/core';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faChevronDown,faTachometerAlt,faBookReader,faBuilding,faUserTie,faFileInvoice,faSitemap,faRadiation,
          faSignOutAlt,faBusinessTime,faRegistered,faBinoculars,faEnvelope,faListOl,faPrayingHands,faWindowRestore, 
          faHandHoldingUsd,faPlay, faSignature,faEye, faFileUpload, faIdCardAlt, faCogs, faDownload, faMailBulk, faExchangeAlt, 
          faUserCheck, faUserPlus, faUserMinus, faCoins, faStreetView, faCaretLeft, faCalculator, faUserGraduate,
           faUniversity, faArrowRight, faThumbsUp, faFileDownload, faScroll, faPoll, faFilter, faHandshake, 
           faFileContract, faStrikethrough,faUserCog, faFileArchive, faIdCard, faHatCowboy, faIndustry, faUserTag, faTrafficLight, faTasks, faShareAltSquare
          
      } 
    from '@fortawesome/free-solid-svg-icons';

import { faFilePdf,faFileExcel,faPlusSquare,faEdit,faTrashAlt,faUserCircle,faShareSquare,faPaperPlane 
      } from '@fortawesome/free-regular-svg-icons';

@NgModule({
  imports: [ FontAwesomeModule ],
  exports: [ FontAwesomeModule ]
})
export class FontIconModule {
  constructor(library: FaIconLibrary) {
    // add icons to the library for convenient access in other components
    library.addIcons(faChevronDown,faTachometerAlt,faBookReader,faBuilding,faUserTie,faFileInvoice,faSitemap,faRadiation,
      faSignOutAlt,faBusinessTime,faRegistered,faBinoculars,faEnvelope,faListOl,faPrayingHands,faWindowRestore,
      faHandHoldingUsd,faPlay,faSignature,faEye,faFileUpload,faIdCardAlt,faCogs,faDownload,faMailBulk,faExchangeAlt,
      faUserPlus,faUserMinus,faCoins,faStreetView, faCaretLeft,faCalculator,faUserGraduate,faUniversity,faArrowRight,
      faThumbsUp,faFileDownload,faScroll,faPoll,faFilter,faHandshake,faFileContract,faStrikethrough,faUserCog,
      faFileArchive,faIdCard,faHatCowboy,faIndustry,faUserTag,faTrafficLight,faTasks,faShareAltSquare );

    library.addIcons(faFilePdf,faFileExcel,faPlusSquare,faEdit,faTrashAlt,faUserCircle,faShareSquare,faPaperPlane);
  }
}