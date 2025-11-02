# GitHub Integration Client

A beautiful, modern web interface for connecting to GitHub and viewing your profile and repositories.

## Features

- **Secure Authentication**: Password-protected input for GitHub Personal Access Token
- **Profile Display**: View your GitHub profile with avatar, bio, stats, and contact information
- **Repository Listing**: Browse your repositories with detailed information
- **Modern UI**: Built with TailwindCSS for a clean, responsive design
- **Error Handling**: Comprehensive error messages and loading states

## How to Use

1. **Get a GitHub Personal Access Token**:

   - Go to [GitHub Settings > Tokens](https://github.com/settings/tokens)
   - Click "Generate new token (classic)"
   - Select the necessary scopes (at minimum: `read:user` and `repo`)
   - Copy the generated token

2. **Connect to GitHub**:

   - Enter your Personal Access Token in the input field
   - Click "Connect to GitHub"
   - Your profile will be displayed with your avatar and information

3. **View Repositories**:
   - After successfully connecting, click "Load My Repositories"
   - Browse through your repositories with details like language, stars, forks, and last update

## Security Features

- API key is hidden by default (password field)
- Toggle visibility with the eye icon
- API key is cleared from the input after successful connection
- No API key is stored in browser storage

## API Endpoints Used

- `GET /user?apiKey={token}` - Fetches user profile
- `GET /repositories?apiKey={token}` - Fetches user repositories

## Technologies Used

- **HTML5** - Semantic markup
- **TailwindCSS** - Utility-first CSS framework
- **Vanilla JavaScript** - No frameworks, pure ES6+
- **GitHub API v3** - REST API for GitHub data

## Browser Compatibility

Works in all modern browsers that support:

- ES6+ JavaScript features
- Fetch API
- CSS Grid and Flexbox
- CSS Custom Properties

## Development

To run this locally:

1. Ensure the API server is running
2. Navigate to the client directory
3. Open `index.html` in your browser
4. Or serve it through the API server which serves static files from this directory

## Notes

- The application requires a running API server to function
- GitHub API has rate limits that may affect usage
- All API calls are made through the backend to avoid CORS issues
