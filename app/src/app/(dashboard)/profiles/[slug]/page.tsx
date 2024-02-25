type EncodeProfilePageProps = {
  params: {
    slug: string
  }
}

const EncodeProfilePage: React.FC<EncodeProfilePageProps> = ({ params }) => <p>profile {params.slug}</p>

export default EncodeProfilePage
