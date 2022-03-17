import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found-error',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {
  public notFoundText: string = '404 - The page you are looking for is not found!';

  constructor() { }

  ngOnInit(): void {
  }

}
