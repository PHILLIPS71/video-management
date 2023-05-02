module.exports = {
  extends: ['airbnb', 'airbnb-typescript', 'airbnb/hooks', 'prettier'],
  rules: {
    'react/jsx-uses-react': 'off',
    'react/jsx-props-no-spreading': 'off',
    'react/react-in-jsx-scope': 'off',
    'react/prop-types': 'off',
    'react/function-component-definition': [2, { namedComponents: 'arrow-function' }],
    'no-console': ['error', { allow: ['warn', 'error'] }],
    'no-underscore-dangle': ['error', { allow: ['__typename'] }],
    'sort-imports': [
      'error',
      {
        ignoreDeclarationSort: true,
        ignoreMemberSort: false,
      },
    ],
    'import/extensions': 'off',
    'import/prefer-default-export': 'off',
    'import/order': [
      'error',
      {
        groups: ['type', 'builtin', 'external', 'parent', 'sibling', 'internal', 'index', 'object'],
        'newlines-between': 'always',
        alphabetize: { order: 'asc', caseInsensitive: true },
      },
    ],
  },
}
