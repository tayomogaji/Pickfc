import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzAffixModule } from 'ng-zorro-antd/affix';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NzDrawerModule,
    NzPopconfirmModule,
    NzPopoverModule,
    NzInputNumberModule,
    NzTabsModule,
    NzAffixModule,
  ],
  exports: [
    CommonModule,
    NzDrawerModule,
    NzPopconfirmModule,
    NzPopoverModule,
    NzInputNumberModule,
    NzTabsModule,
    NzAffixModule,
  ],
  providers: [
    { provide: NzMessageService },
    { provide: NZ_I18N, useValue: en_US },
  ],
})
export class ZorroModule { }
