import { Component, OnInit } from '@angular/core';
import { PowerInfo, Temperatures, WaterInfo } from './dataclasses';
import { HouseCoreService } from './house-core.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'house-info';
  
  powerInfo : PowerInfo;
  waterInfo : WaterInfo;
  temperatures : Temperatures;
  
  interval : any;

  constructor(private houseCoreService: HouseCoreService)
  {

  }

  date(datestring : string) : Date {
    return datestring != "" ? new Date(datestring) : new Date();
  }

  ngOnInit(): void {
    this.update();
    this.interval = setInterval(() => { this.update() }, 3000);
  }

  update() {
    this.houseCoreService.getPowerData().subscribe((x : PowerInfo) => this.powerInfo = x);
    this.houseCoreService.getWaterData().subscribe((x : WaterInfo) => this.waterInfo = x);
    this.houseCoreService.getTemperatureData().subscribe((x : Temperatures) => this.temperatures = x);
    
  }
}
