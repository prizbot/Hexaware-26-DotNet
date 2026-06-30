# Student Learning Portal

A React Router based Student Learning Portal with public and protected pages.

## How to Run

1. Install dependencies:
   ```
   npm install
   ```

2. Start the dev server:
   ```
   npm run dev
   ```

3. Open the URL shown in the terminal (usually http://localhost:5173)

## Login Credentials

- Username: `student`
- Password: `student123`

## Features

- Public pages: Home, About, Courses, Course Details, Contact, Login
- Protected pages (require login): Dashboard, Profile, My Courses, Settings
- Nested routing inside Dashboard
- Dynamic routing for course details (`/courses/:id`)
- Protected route redirection to Login when not authenticated
- Wildcard 404 page for invalid URLs
- Login state persisted in localStorage
- Active link highlighting using NavLink
- Programmatic navigation using useNavigate (login, logout, back, 404 redirect)
