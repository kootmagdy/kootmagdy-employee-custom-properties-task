import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule],
  template: `
    <header class="nav">
      <div class="brand">Employees Admin</div>
      <nav>
        <a routerLink="/properties/new" routerLinkActive="active">New Property</a>
        <a routerLink="/employees/new" routerLinkActive="active">New Employee</a>
        <a routerLink="/employees" routerLinkActive="active">Employees</a>
      </nav>
    </header>
  `,
  styles:[`
    .nav{position:sticky;top:0;z-index:10;background:#0f172a;color:#fff;display:flex;align-items:center;justify-content:space-between;padding:12px 20px}
    .brand{font-weight:700}
    nav{display:flex;gap:10px}
    a{color:#e5e7eb;text-decoration:none;padding:8px 12px;border-radius:8px}
    a.active, a:hover{background:rgba(255,255,255,.12);color:#fff}
  `]
})
export class NavbarComponent {}
