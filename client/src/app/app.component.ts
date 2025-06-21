import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'client';
  http = inject(HttpClient);
  users:any;

  constructor(){

  }
  ngOnInit() {
    this.http.get('https://localhost:5076/api/users').subscribe({
      next: response => {
        this.users = response;
      }
      , error: (error) => {
        console.error('Error fetching users:', error);
      }
      , complete: () => {
        console.log('User fetch complete');
      }
  }
);

//console.log(this.users);


  }
}